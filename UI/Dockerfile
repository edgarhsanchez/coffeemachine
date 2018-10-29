FROM scratch

COPY main /main

COPY dist/UI/* /public/

ENV PORT=80
ENV VERSION=1.0.0

EXPOSE $PORT

CMD ["/main"]