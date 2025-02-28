# Copyright 2021 Google LLC
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# https://mcr.microsoft.com/v2/dotnet/sdk/tags/list
FROM mcr.microsoft.com/dotnet/sdk:7.0 as builder
WORKDIR /app
COPY paymentintegration.csproj .
RUN dotnet restore paymentintegration.csproj -r linux-musl-x64
COPY . .
RUN dotnet publish paymentintegration.csproj -r linux-musl-x64 -c release -o /paymentintegration --no-restore

# https://mcr.microsoft.com/v2/dotnet/runtime-deps/tags/list
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0.1-alpine3.16-amd64@sha256:3674a509a2735aaaeef7ba9438d8fc0ba02d0b303fbeaf8b1a27669f08327f1b as without-grpc-health-probe-bin

WORKDIR /app
COPY --from=builder /paymentintegration .
EXPOSE 7373
ENV DOTNET_EnableDiagnostics=0 \
    ASPNETCORE_URLS=http://*:7373
USER 1000
ENTRYPOINT ["/app/paymentintegration"]

FROM without-grpc-health-probe-bin
USER root
# renovate: datasource=github-releases depName=grpc-ecosystem/grpc-health-probe
RUN GRPC_HEALTH_PROBE_VERSION=v0.4.14 && \
    wget -qO/bin/grpc_health_probe https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 && \
    chmod +x /bin/grpc_health_probe
USER 1000
