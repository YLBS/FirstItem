// globalVariables.ts
import { createApp } from 'vue';
 
const app = createApp({}); // 创建一个空的应用程序对象
app.config.globalProperties.$myGlobalVariable = 'Hello World!'; // 设置全局变量
export default app;