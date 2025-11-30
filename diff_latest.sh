#!/usr/bin/env bash

# Get the path of "old" file for the diff.  This is the newest file
# stored in the `output/` directory, which should be from a previous
# build of Lost Skies Data Dump (and/or a previous build of Lost
# Skies).
OLD="$(ls -t output/*.json | head -1)"

# The path of the "new" file for the diff.  This should be the data file
# produced by the most recent run of Lost Skies Data Dump.
NEW='C:/Program Files (x86)/Steam/steamapps/common/Wild Skies/LostSkies_Data/LostSkiesDataDump/data.json'

# Diff the files and show the result using `less`.
diff --color=always "$OLD" "$NEW" | less -R
