import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5226",
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("Token") || sessionStorage.getItem("Token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(new Error(error.message || "Request failed"));
  }
);

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      console.warn("Unauthorized! Redirecting to login...");
      localStorage.removeItem("Token");
      sessionStorage.removeItem("Token");
      localStorage.removeItem("role");
      sessionStorage.removeItem("role");
      localStorage.removeItem("userName");
      sessionStorage.removeItem("userName");
      // Do not redirect if we are already on login page or if this is a login attempt failure
      if (!window.location.pathname.includes('/login')) {
        window.location.href = "/login";
      }
    }
    return Promise.reject(new Error(error.message || "Request failed"));
  }
);

export default api;
