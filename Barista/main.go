package main

import (
	"os"
	"time"

	"github.com/gin-contrib/gzip"
	"github.com/gin-gonic/gin"
	_ "github.com/json-iterator/go"
)

func main() {
	router := gin.Default()
	router.Use(gzip.Gzip(gzip.DefaultCompression))

	router.POST("/order", orderCoffeeHandler)

	router.Run(":" + os.Getenv("PORT"))
}

func orderCoffeeHandler(c *gin.Context) {
	result := make(chan gin.H)
	go func() {
		time.Sleep(10 * time.Second)

		result <- gin.H{
			"status": "ok",
			"orders": 1,
		}
	}()
	c.JSON(200, <-result)
}
