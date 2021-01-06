import axios, { AxiosError, AxiosRequestConfig } from "axios";
import { Ref } from 'vue'



export const baseURL = process.env.NODE_ENV === 'development'
    ? "http://127.0.0.1:5010/api/v1"
    : "/api/v1"

console.log("***** process.env.NODE_ENV", process.env.NODE_ENV, baseURL)

export const axiosInstance = axios.create({
    baseURL: baseURL,
    timeout: 5000,
    withCredentials: true
});

// Use Bearer accessToken from localStorage if provided
axiosInstance.interceptors.request.use(
    config => {
        const accessToken = localStorage.getItem("accessToken");
        if (accessToken) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    },
    error => Promise.reject(error)
);

// Reactive request
export default function (status: Ref<{ loading: boolean; state: number; message: string }>) {

    async function request<T = any>(
        method: "GET" | "PUT" | "POST" | "DELETE",
        url: string,
        data?: any,
        config?: AxiosRequestConfig) {

        config = { method, url, data, ...config }

        try {
            status.value.loading = true;
            const response = await axiosInstance.request<T>(config)
            status.value.state = response.status;
            status.value.message = response.statusText;
            return response.data
        }
        catch (error) {
            const err = error as AxiosError
            if (err?.response) {
                status.value.state = err.response.status;
                status.value.message = err.response.statusText
            }
            else {
                status.value.state = error.response?.status || error.code === 'ECONNABORTED' ? 408 : 500
                status.value.message =
                    error.response?.data?.error
                    || error.response?.data?.message
                    || error.message
                    || "Network Error";
            }
        }
        finally {
            status.value.loading = false
        }
    }
    return { request }
}
