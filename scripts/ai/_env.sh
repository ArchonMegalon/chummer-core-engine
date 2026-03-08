#!/usr/bin/env bash
set -euo pipefail
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_CLI_HOME="${DOTNET_CLI_HOME:-$HOME/.dotnet-cli}"
export NUGET_PACKAGES="${NUGET_PACKAGES:-$HOME/.nuget/packages}"
export TMPDIR="${TMPDIR:-$HOME/.cache/chummer-tmp}"
mkdir -p /tmp/.dotnet/shm "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" "$TMPDIR"
