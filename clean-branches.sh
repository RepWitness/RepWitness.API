#!/bin/bash

echo "🔄 Cleaning remote references..."
git fetch --prune

echo "🔍 Finding deleted remote branches..."
branches=$(git branch -vv | grep ': gone]' | awk '{print $1}')

if [ -z "$branches" ]; then
  echo "✅ No deleted branches found."
  exit 0
fi

echo "🗑 Deleting local branches:"
echo "$branches"

for branch in $branches; do
  git branch -d "$branch"
done

echo "✅ Cleanup complete."