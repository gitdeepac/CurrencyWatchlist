import axios from "axios";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: { "Content-Type": "application/json" },
});

export const rateApi = {
  refreshRate: async () => {
    try {
      const res = await api.post("/Rates/refresh");
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  refreshRateWithInput: async (baseCurrency: string, quoteCurrency: string) => {
    try {
      const res = await api.get(`/Rates/latest?baseCur=${baseCurrency}&quoteCur=${quoteCurrency}`);
      return { success: true, data: res.data };
    } catch (error: any) {
      return {
        success: false,
        message: error.response?.data?.message || "Something went wrong",
      };
    }
  },

  refreshRateWithInputHistorical: async (baseCurrency: string, quoteCurrency: string, startDate: string, endDate:string) => {
    try {
      const res = await api.get(`/Rates/history?baseCur=${baseCurrency}&quoteCur=${quoteCurrency}&startDate=${startDate}&endDate=${endDate}`);
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
