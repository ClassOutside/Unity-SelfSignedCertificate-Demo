const express = require("express");

class TestController {
  static getData(req, res) {
    const data = ModelService.getData();
    res.json(data);
  }

  static getJSONResponse(req, res) {
    const jsonResponse = [
      {
        testValue: "Hello World!",
      },
    ];

    res.json(jsonResponse);
  }

  static getRoutes() {
    const router = express.Router();
    router.get("/endpoint", this.getJSONResponse);
    return router;
  }
}

module.exports = TestController;
