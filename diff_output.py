#!/usr/bin/env python3

import argparse
import re
import subprocess
from pathlib import Path
from typing import NotRequired, TypedDict, assert_never

DEFAULT_TO_FILE = Path(
    "C:/Program Files (x86)/Steam/steamapps/common/Wild Skies/LostSkies_Data/"
    "LostSkiesDataDump/data.json"
)
OUTPUT_DIR = Path("output")
JSON_GLOB_PATTERN = "*.json"

PARSER = argparse.ArgumentParser(
    description="A tool for diffing Lost Skies Data Dump output."
)
PARSER.add_argument(
    nargs="?",
    default=None,
    type=Path,
    help="The from file for comparison.",
    metavar="FROM-FILE",
    dest="from_file",
)
PARSER.add_argument(
    nargs="?",
    default=DEFAULT_TO_FILE,
    type=Path,
    help="The to file for comparison.",
    metavar="TO-FILE",
    dest="to_file",
)


class LineNumbers(TypedDict):
    from_start: int
    from_stop: NotRequired[int]
    to_start: int
    to_stop: NotRequired[int]


class SubDiff(TypedDict):
    line_numbers: LineNumbers
    from_lines: NotRequired[list[str]]
    to_lines: NotRequired[list[str]]


RE_LINE_NUMS = re.compile(
    r"""
    (?P<from_start>\d+)
    (?:,(?P<from_stop>\d+))?
    c
    (?P<to_start>\d+)
    (?:,(?P<to_stop>\d+))?
    """,
    re.VERBOSE,
)
RE_FLOAT = re.compile(r"-?\d+(?:\.\d+)?(?:E[+-]\d+)?")


def main() -> int:
    args = PARSER.parse_args()
    if args.from_file is None:
        args.from_file = get_default_to_file()
    proc = diff_files(args)
    diff = parse_diff(proc.stdout)
    for sub_diff in diff:
        if should_ignore_sub_diff(sub_diff):
            continue
        line_nums = sub_diff["line_numbers"]
        to_stop = line_nums.get("to_stop")
        parts = [str(line_nums["from_start"])]
        if (from_stop := line_nums.get("from_stop")) is not None:
            parts.append(f",{from_stop}")
        parts.extend(f"c{line_nums['to_start']}")
        if (to_stop := line_nums.get("to_stop")) is not None:
            parts.append(f",{to_stop}")
        print("".join(parts))
        if (lines := sub_diff.get("from_lines")) is not None:
            for line in lines:
                print(f"< {line}")
        print("---")
        if (lines := sub_diff.get("to_lines")) is not None:
            for line in lines:
                print(f"> {line}")
    return proc.returncode


def get_default_to_file() -> Path:
    files = sorted(
        OUTPUT_DIR.glob(JSON_GLOB_PATTERN), key=get_m_time, reverse=True
    )
    assert len(files) > 0, list(OUTPUT_DIR.glob("*"))
    return files[0]


def get_m_time(path: Path) -> float:
    return path.stat().st_mtime


def diff_files(args: argparse.Namespace) -> subprocess.CompletedProcess[str]:
    proc = subprocess.run(
        ["diff", args.from_file, args.to_file],
        capture_output=True,
        check=False,
        text=True,
    )
    match proc.returncode:
        case 0:  # Inputs are the same.
            assert len(proc.stdout) == 0 and len(proc.stderr) == 0, proc
            exit(0)
        case 1:  # Inputs are different.
            assert len(proc.stderr) == 0, proc
            return proc
        case 2:  # "Trouble".
            proc.check_returncode()
            assert False, proc
        case _:
            raise ValueError(
                f"unexpected returncode ({proc.returncode}) from diff"
            )
    assert_never(proc.returncode)


def parse_diff(text: str) -> list[SubDiff]:
    parts: list[SubDiff] = []
    part: SubDiff | None = None
    reached_sep = False
    for num, line in enumerate(text.splitlines(), 1):
        assert len(line) > 1, f"line {num}: {line!r}"
        match line[:2]:
            case "< ":
                assert (not reached_sep) and part is not None, (
                    f"{part=}; line {num}: {line!r}"
                )
                if "from_lines" not in part:
                    part["from_lines"] = [line[2:]]
                else:
                    part["from_lines"].append(line[2:])
            case "> ":
                assert reached_sep and part is not None, (
                    f"{part=}; line {num}: {line!r}"
                )
                if "to_lines" not in part:
                    part["to_lines"] = [line[2:]]
                else:
                    part["to_lines"].append(line[2:])
            case "--":
                assert line == "---", f"line {num}: {line!r}"
                reached_sep = True
            case _:
                if part is not None:
                    parts.append(part)
                part = SubDiff(line_numbers=parse_diff_line_numbers(num, line))
                reached_sep = False
    if part is not None:
        parts.append(part)
    return parts


def parse_diff_line_numbers(num: int, line: str) -> LineNumbers:
    if (m := RE_LINE_NUMS.fullmatch(line)) is None:
        raise ValueError(f"could not parse line {num}: {line!r}")
    parts = LineNumbers(
        from_start=int(m["from_start"]), to_start=int(m["to_start"])
    )
    if m["from_stop"] is not None:
        parts["from_stop"] = int(m["from_stop"])
    if m["to_stop"] is not None:
        parts["to_stop"] = int(m["to_stop"])
    return parts


def should_ignore_sub_diff(sub_diff: SubDiff) -> bool:
    from_lines = sub_diff.get("from_lines")
    to_lines = sub_diff.get("to_lines")
    if from_lines is None or len(from_lines) != 2:
        return False
    if to_lines is None or len(to_lines) != 2:
        return False
    if from_lines[0][-1] != ",":
        return False
    if from_lines[0][-1] != ",":
        return False
    if from_lines[0][:-1] != from_lines[1]:
        return False
    if to_lines[0][:-1] != to_lines[1]:
        return False
    if RE_FLOAT.fullmatch(from_lines[1].lstrip()) is None:
        return False
    if RE_FLOAT.fullmatch(to_lines[1].lstrip()) is None:
        return False
    line_nums = sub_diff["line_numbers"]
    from_start = line_nums["from_start"]
    if (from_stop := line_nums.get("from_stop")) is None:
        return False
    to_start = line_nums["to_start"]
    if (to_stop := line_nums.get("to_stop")) is None:
        return False
    if from_stop - from_start != 1:
        return False
    if to_stop - to_start != 1:
        return False
    return True


if __name__ == "__main__":
    exit(main())
