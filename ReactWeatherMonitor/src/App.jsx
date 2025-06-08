// src/App.jsx
import { useState, useCallback } from 'react'; // No need to import React for JSX in React 17+ with new transform
import axios from 'axios';
import './App.css'; // We'll create this file for styling

// IMPORTANT: Replace with your actual WeatherAPI.com API key
const WEATHER_API_KEY = 'a107f8e477e04a5bbb9170145252105';
const WEATHER_API_URL = 'https://api.weatherapi.com/v1/current.json';

// You can import the apm instance if you want to manually create transactions or capture errors.
// For many cases, the auto-instrumentation will handle things.
// import apm from './apm';

function App() {
  const [city, setCity] = useState('');
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchWeather = useCallback(async () => {
    if (!city.trim()) {
      setError('Please enter a city name.');
      setWeather(null);
      return;
    }

    setLoading(true);
    setError('');
    setWeather(null);

    // Example: Manually starting a custom transaction (optional)
    // const transaction = apm.startTransaction('fetch-weather-data', 'custom');

    try {
      const response = await axios.get(WEATHER_API_URL, {
        params: {
          key: WEATHER_API_KEY,
          q: city,
        },
      });
      setWeather(response.data);
      // transaction?.setOutcome('success'); // If manual transaction
    } catch (err) {
      console.error('Failed to fetch weather:', err);
      const errorMessage =
        err.response?.data?.error?.message ||
        'Failed to fetch weather. Check the city name, API key, or network.';
      setError(errorMessage);

      // Example: Manually capturing an error with more context (optional)
      // const customError = new Error(`Weather API call failed for city: ${city}`);
      // apm.captureError(customError, {
      //   custom: {
      //     city_searched: city,
      //     api_error_message: err.message,
      //     api_status: err.response?.status,
      //   }
      // });
      // transaction?.setOutcome('failure'); // If manual transaction
    } finally {
      setLoading(false);
      // transaction?.end(); // If manual transaction
    }
  }, [city]); // Add WEATHER_API_KEY if it could change, but it's a const here

  const handleSubmit = (e) => {
    e.preventDefault(); // Prevent default form submission which reloads the page
    fetchWeather();
  };

  return (
    <div className="app-container">
      <h1>React 19 Weather App</h1>
      <p>Monitoring with Elastic APM</p>

      <form onSubmit={handleSubmit} className="weather-form">
        <input
          type="text"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          placeholder="Enter city name (e.g., London)"
          aria-label="City name"
          disabled={loading}
        />
        <button type="submit" disabled={loading || !city.trim()}>
          {loading ? 'Searching...' : 'Get Weather'}
        </button>
      </form>

      {error && <p className="error-message">{error}</p>}

      {weather && weather.current && weather.location && (
        <div className="weather-info">
          <h2>{weather.location.name}, {weather.location.country}</h2>
          <p><strong>Temperature:</strong> {weather.current.temp_c}°C ({weather.current.temp_f}°F)</p>
          <p><strong>Condition:</strong> {weather.current.condition.text}</p>
          <img
            src={weather.current.condition.icon}
            alt={weather.current.condition.text}
            title={weather.current.condition.text}
          />
          <p><strong>Humidity:</strong> {weather.current.humidity}%</p>
          <p><strong>Wind:</strong> {weather.current.wind_kph} kph ({weather.current.wind_dir})</p>
          <p><strong>Last updated:</strong> {weather.current.last_updated}</p>
        </div>
      )}
    </div>
  );
}

export default App;