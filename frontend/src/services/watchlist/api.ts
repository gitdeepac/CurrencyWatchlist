import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:5001/api",
  headers: { "Content-Type": "application/json" },
});

export const watchlistApi = {
  getAll: async () => {
    const res = await api.get("/watchlists");
    return res.data;
  },

  getById: async (id: number) => {
    const res = await api.get(`/watchlists/${id}`);
    return res.data;
  },

  create: async (data: { watchlistName: string }) => {
    const res = await api.post("/watchlists", data);
    return res.data;
  },

  delete: async (id: number) => {
    const res = await api.delete(`/watchlists/${id}`);
    return res.data;
  },
};

export default api;
