// src/apm.js
import { init as initApm } from "@elastic/apm-rum";

const apm = initApm({
  serviceName: "weather-app-react-frontend",

  serverUrl: "http://localhost:8200",

  environment: "development",

  logLevel: "debug",

  distributedTracingOrigins: ["http://localhost:5173"],

  breakdownMetrics: true,
  instrument: true,
  centralConfig: true,
  captureHeaders: true,
  captureBody: "all",
});

export default apm;
