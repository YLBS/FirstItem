import axios,{AxiosInstance,AxiosRequestConfig,AxiosPromise,AxiosResponse} from 'axios';
import {ElMessage} from 'element-plus'
import { useRouter } from 'vue-router'
const router=useRouter();
const instance = axios.create({
  baseURL: 'http://localhost:5246/',
});
//响应拦截器
instance.interceptors.response.use(response=>{
  //debugger
    if(response.status==200){
        return response.data;
    }
    else{
      console.log(response);
    }
},error => {
  if(error.response.status){
    if(error.response.status==401){
      ElMessage({
        message:'请先登录',
        type:'warning'
      })
     
    } 
    console.log("请求错误时的处理"+error);
  }}
  )

// 添加请求拦截器
instance.interceptors.request.use(
  config => {
    // 在请求中添加 Token
    const token = sessionStorage.getItem('Cartoken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    // 请求错误时的处理
    console.log("请求错误时的处理"+error);
    return Promise.reject(error);
  }
);
export default instance;