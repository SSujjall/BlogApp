import axios from "../../../service/AxiosInstance";

export const login = async (email, password) => {
    const response = await axios.post("/auth/login", { email, password });
    return response.data;
  };
  
  export const signup = async (email, password) => {
    const response = await axios.post("/auth/signup", { email, password });
    return response.data;
  };