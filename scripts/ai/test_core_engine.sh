#!/usr/bin/env bash
set -euo pipefail
source "$(dirname "$0")/_env.sh"
dotnet build Chummer.CoreEngine.Tests/Chummer.CoreEngine.Tests.csproj --nologo -m:1 "$@"
dotnet Chummer.CoreEngine.Tests/bin/Debug/net10.0/Chummer.CoreEngine.Tests.dll
