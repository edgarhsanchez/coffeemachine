package main

import (
	"os"

	"github.com/gin-contrib/static"
	"github.com/gin-gonic/contrib/gzip"
	"github.com/gin-gonic/gin"
)

func main() {

	router := gin.Default()
	router.Use(gzip.Gzip(gzip.DefaultCompression))

	router.Use(static.Serve("/", static.LocalFile("/public", true)))
	router.Use(static.Serve("/public", static.LocalFile("/public", true)))

	router.Run(":" + os.Getenv("PORT"))
}
