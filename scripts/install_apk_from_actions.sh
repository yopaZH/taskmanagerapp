#!/bin/bash

# Script to download latest APK from GitHub Actions and install via ADB
# Usage: ./install_apk_from_actions.sh [github-token] [owner/repo]

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
REPO="${2:-styopa/TaskManagerApp}"
TOKEN="${1:-}"
WORKFLOW_NAME="android-build.yml"
ARTIFACT_NAME="android-apk-release"

echo -e "${YELLOW}=== Task Manager App - APK Installer ===${NC}"
echo "Repository: $REPO"
echo "Workflow: $WORKFLOW_NAME"
echo ""

# Check prerequisites
if ! command -v adb &> /dev/null; then
    echo -e "${RED}Error: adb not found. Install Android platform-tools.${NC}"
    exit 1
fi

if ! command -v gh &> /dev/null; then
    echo -e "${RED}Error: gh (GitHub CLI) not found.${NC}"
    echo "Install from: https://cli.github.com/"
    exit 1
fi

# Check if phone is connected
echo -e "${YELLOW}Checking for connected devices...${NC}"
DEVICES=$(adb devices | grep -v "^$" | grep -v "List of" | wc -l)
if [ "$DEVICES" -lt 2 ]; then
    echo -e "${RED}No Android devices found. Connect phone via USB and enable USB debugging.${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Device found${NC}"
adb devices

# Get latest successful workflow run
echo -e "${YELLOW}Fetching latest workflow run...${NC}"
LATEST=$(gh run list \
  --repo "$REPO" \
  --workflow "$WORKFLOW_NAME" \
  --status success \
  --json databaseId \
  -q '.[0].databaseId' 2>/dev/null || echo "")

if [ -z "$LATEST" ]; then
    echo -e "${RED}No successful workflow runs found.${NC}"
    echo "Push changes to trigger the build, or visit: https://github.com/$REPO/actions"
    exit 1
fi

echo -e "${GREEN}✓ Found run: $LATEST${NC}"

# Download artifact
echo -e "${YELLOW}Downloading APK artifact...${NC}"
TEMP_DIR=$(mktemp -d)
trap "rm -rf $TEMP_DIR" EXIT

gh run download "$LATEST" \
  --repo "$REPO" \
  --name "$ARTIFACT_NAME" \
  --dir "$TEMP_DIR" 2>/dev/null

# Find APK file
APK_FILE=$(find "$TEMP_DIR" -name "*.apk" -type f | head -1)

if [ -z "$APK_FILE" ]; then
    echo -e "${RED}APK file not found in artifacts.${NC}"
    echo "Contents of downloaded files:"
    ls -la "$TEMP_DIR"
    exit 1
fi

echo -e "${GREEN}✓ Downloaded: $(basename $APK_FILE)${NC}"

# Install APK
echo -e "${YELLOW}Installing APK...${NC}"
adb install -r "$APK_FILE"

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Installation successful!${NC}"
    echo ""
    echo "App is ready to launch on your device:"
    echo "  • Look for 'Task Manager' in your app list"
    echo "  • Or use: adb shell am start -n com.taskmanager.app/com.taskmanager.app.MainActivity"
else
    echo -e "${RED}Installation failed!${NC}"
    exit 1
fi
