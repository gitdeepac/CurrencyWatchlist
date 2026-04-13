import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5109/api",
  headers: { "Content-Type": "application/json" },
});

export const watchlistApi = {
  getAll: async () => {
    try {
      const res = await api.get("/watchlists");
      return { success: true, data: res.data.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  getById: async (id: number) => {
    try {
      const res = await api.get(`/watchlists/${id}`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  create: async (data: { name: string }) => {
    try {
      const res = await api.post("/watchlists", data);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  delete: async (id: number) => {
    try {
      const res = await api.delete(`/watchlists/${id}`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },
};

export default api;
