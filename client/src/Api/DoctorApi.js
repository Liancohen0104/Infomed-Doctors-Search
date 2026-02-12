import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5153/api",
  headers: { "Content-Type": "application/json" },
});

export async function fetchDoctors() {
  const res = await api.get("/doctors");
  return res.data;
}

export async function fetchDoctorById(id) {
  const res = await api.get(`/doctors/${id}`);
  return res.data;
}

export async function fetchActiveDoctors() {
  const res = await api.get("/doctors/active");
  return res.data;
}

export async function fetchPromotedDoctors() {
  const res = await api.get("/doctors/promoted");
  return res.data;
}