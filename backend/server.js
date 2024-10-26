const express = require("express");
const https = require("https");
const fs = require("fs");
const cors = require("cors");
const TestController = require("./controllers/TestController");

const app = express();

const options = {
  key: fs.readFileSync("./keys/key.pem"),
  cert: fs.readFileSync("./keys/cert.pem"),
};

app.use(cors());

app.use("/test", TestController.getRoutes());

// Create HTTPS server
const PORT = 3001;
https.createServer(options, app).listen(PORT, () => {
  console.log(`HTTPS Server running on https://localhost:${PORT}`);
});
