FROM alpine:3.7

RUN apk add --no-cache \
        ca-certificates \
        \
        # .NET Core dependencies
        krb5-libs \
        libgcc \
        libintl \
        libssl1.0 \
        libstdc++ \
        tzdata \
        userspace-rcu \
        zlib \
    && apk -X https://dl-cdn.alpinelinux.org/alpine/edge/main add --no-cache \
        lttng-ust

# Enable detection of running in a container
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

ENV DOTNET_VERSION 2.1.7

RUN apk add --no-cache --virtual .build-deps \
        openssl \
    && wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='e362a0497fcd524ea1d2fa512bf439e54ca487660a3f3e3e1b5b78b98efb4c5bfe0c4a092501adcc02ade2436ae656b9effcce738da5f49a8333ab3d058a5ad7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz \
&& apk del .build-deps

RUN addgroup -g 1001 -S AIGroup && \
	adduser -u 1001 -S AIUser -G AIGroup

USER AIUser:AIGroup