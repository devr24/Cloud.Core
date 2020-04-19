FROM alpine:3.10.3

RUN apk add --no-cache \
    ca-certificates \
    \
    # .NET Core dependencies
    krb5-libs \
    libgcc \
    libintl \
    libssl1.1 \
    libstdc++ \
    zlib

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Install .NET Core
RUN dotnet_version=3.1.1 \
    && wget -O dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='73a29e7f6c662beabd70d20bb4331c7a4d3fe4096002cb802f2db08946fd918681151e1cfcda537210231883852364a5c7707e11bbc28d8613ff4ec1a8e98015' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -oxzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# TODO: Add user and group here