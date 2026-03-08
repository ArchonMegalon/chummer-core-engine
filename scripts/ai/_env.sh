#!/usr/bin/env bash
set -euo pipefail
repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
cache_root="${CHUMMER_AI_CACHE_ROOT:-$repo_root/.tmp/ai}"
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_NOLOGO=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_CLI_HOME="${DOTNET_CLI_HOME:-$cache_root/.dotnet-cli}"
export NUGET_PACKAGES="${NUGET_PACKAGES:-$cache_root/.nuget/packages}"
export TMPDIR="${TMPDIR:-$cache_root/tmp}"
mkdir -p /tmp/.dotnet/shm "$DOTNET_CLI_HOME" "$NUGET_PACKAGES" "$TMPDIR"
