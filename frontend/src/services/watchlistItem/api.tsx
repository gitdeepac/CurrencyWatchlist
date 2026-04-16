import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: { "Content-Type": "application/json" },
});

export const watchlistItemApi = {
  
  getByWatchlistIdAll: async (watchlistId: number) => {
    try {
      const res = await api.get(`/Watchlists/${watchlistId}/items`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },
  getByWatchlistItemWithItemId: async (watchlistId: number, wcItemId: number) => {
    try {
      const res = await api.get(`/watchlistsItems/${watchlistId}/items/${wcItemId}`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  create: async (watchlistId: number, data: { watchlistId: number, baseCurrency: string, quoteCurrency: string }) => {
    try {
      const res = await api.post(`/Watchlists/${watchlistId}/items`, data);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  delete: async (watchlistId: number, wcItemId: number) => {
    try {
      const res = await api.delete(`/Watchlists/${watchlistId}/items/${wcItemId}`);
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
