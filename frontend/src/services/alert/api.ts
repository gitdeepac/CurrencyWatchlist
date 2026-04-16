import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: { "Content-Type": "application/json" },
});

export const alertApi = {
  create: async (data: { watchlistItemId: number, condition: string, threshold: string  }) => {
    try {
      const res = await api.post("/AlertRule", data);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },
  delete: async (alertId: number) => {
    try {
      const res = await api.delete(`/AlertRule/${alertId}`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },
  getAllAlerts: async (alertId: number) => {
    try {
      const res = await api.get(`/AlertRule`);
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
