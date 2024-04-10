import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import {ElMessage} from 'element-plus'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: HomeView,
    meta: { keepAlive: true },
    children: [
      {
        path: '',
        name: 'HomePage',
        meta: { keepAlive: true },
        components: {
          HomeHeader: () => import('../components/home/HomeHeader.vue'),
          //Search: () => import('../components/home/SearchBox.vue'),
          Navig: () => import('../components/home/Navigation.vue')
        }
      },
       {
        path: '',
        name: 'Buy',
        beforeEnter:(to,from,next)=>{
          const token = sessionStorage.getItem('Cartoken');
          if(token==null){
            ElMessage({
              message:'请先登录',
              type:'warning'
            });
            next("/")
          }
          else{
            next()
          }
        },
        components: {
          HomeHeader: () => import('../components/home/HomeHeader.vue'),
          Navig: () => import('../components/home/Buy.vue')
        }
      },
    ],redirect:'HomePage'
  },
  {
    path: '/UserInfo',
    name: 'UserInfo',
    component: () => import('../views/UserInfo.vue'),
    beforeEnter:(to,from,next)=>{
      const token = sessionStorage.getItem('Cartoken');
      if(token==null){
        ElMessage({
          message:'请先登录',
          type:'warning'
        });
        next("/")
      }
      else{
        next()
      }
    },
    children:[
      {
        path:'', 
        name:'ChangePassword', 
        component:()=>import('../components/main/ChangePassword.vue')
      },{
        path:'', 
        name:'ChangeInfo', 
        component:()=>import('../components/main/ChangeInfo.vue')
      },{
        path:'', 
        name:'CarInfo', 
        component:()=>import('../components/main/CarInfo.vue')
      },{
        path:'', 
        name:'ConsignmentOrder', 
        component:()=>import('../components/main/ConsignmentOrder.vue')
      },{
        path:'', 
        name:'AuditConsignmenet', 
        component:()=>import('../components/main/AuditConsignmenet.vue')
      },{
        path:'', 
        name:'SalesTicket', 
        component:()=>import('@/components/main/SalesTicket.vue')
      },{
        path:'', 
        name:'NewSalesTicket', 
        component:()=>import('@/components/main/NewSalesTicket.vue')
      },{
        path:'', 
        name:'SalesTicketList', 
        component:()=>import('@/components/main/SalesTicketList.vue')
      },{
        path:'', 
        name:'SellOrdersList', 
        component:()=>import('@/components/main/SellOrdersList.vue')
      },{
        path:'', 
        name:'SellOrdersList1', 
        component:()=>import('@/components/main/SellOrdersList.vue')
      },{//
        path:'', 
        name:'manageUsers', 
        component:()=>import('@/components/main/ManageUsers.vue')
      },{
        path:'', 
        name:'AuditSales', 
        component:()=>import('@/components/main/AuditSales.vue')
      },{
        path:'', 
        name:'CarInfoList', 
        component:()=>import('@/components/main/CarInfoList.vue')
      },{
        path:'', 
        name:'test1', 
        component:()=>import('../components/main/TestUpload.vue')
      }
    ]
  },
  {
    path: '/AboutView:lo?/:LoginType?',
    name: 'AboutView',
    component: () => import('../views/AboutView.vue')
  },{
    path: '/CarView/:Number?',
    name: 'CarView',
    component: () => import('../views/Car.vue'),
    beforeEnter:(to,from,next)=>{
      const token = sessionStorage.getItem('Cartoken');
      if(token==null){
        ElMessage({
          message:'请先登录',
          type:'warning'
        });
        next("/")
      }
      else{
        next()
      }
    },
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})
/* router.beforeEach((to,from,next)=>{
  
    next();
}) */
export default router
