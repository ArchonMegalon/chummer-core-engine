#!/usr/bin/env bash
set -euo pipefail
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export MSBuildEnableWorkloadResolver=false
export DOTNET_CLI_HOME="${CHUMMER_DOTNET_CLI_HOME:-/tmp/dotnet-cli}"
export NUGET_PACKAGES="${CHUMMER_NUGET_PACKAGES:-/tmp/nuget-packages}"
export TMPDIR="${CHUMMER_TMPDIR:-/tmp/chummer-tmp}"
mkdir -p /tmp/.dotnet/shm "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" "$TMPDIR"
