import apiClient from "../api/apiClient";

export async function login(username, password) {
  const response = await apiClient.post("/account/login", {
    username,
    password,
  });
  return response?.data;
}

export async function register(username, email, password) {
  const response = await apiClient.post("/account/register", {
    username,
    email,
    password,
  });
  return response?.data;
}
