import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5153/api",
  headers: { "Content-Type": "application/json" },
});

export async function sendContactRequest(payload) {
  const res = await api.post("/contacts", payload);
  return res.data;
}