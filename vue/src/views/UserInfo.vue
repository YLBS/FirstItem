<template>
  <div v-loading="loading">
    <el-container class="layout-container-demo" style="height: 700px">
      <el-aside width="200px">
        <el-scrollbar>
          <el-menu :default-openeds="['1', '3']">
            <div v-for="(item, index) in Menu1.dt" :key="index">
              <el-sub-menu :index="item.id" v-if="item.parentId == 0">
                <template #title>
                  {{ item.name }}
                </template>
                <div v-for="(item2, index2) in Menu1.dt" :key="index2">
                  <el-menu-item v-if="item2.parentId == item.id" :index="item2.id" @click="choiceFunction(item2.click)">
                    {{ item2.name }}
                  </el-menu-item>
                </div>
              </el-sub-menu>
            </div>


            <!-- <el-sub-menu index="1">
            <template #title>
              <el-icon>
                <UserFilled />
              </el-icon>个人信息
            </template>
            <el-menu-item index="1-1" @click="ChangePass">修改密码</el-menu-item>

            <el-menu-item index="1-2" @click="ChangeInfo">更新个人信息</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="2">
            <template #title>
              <el-icon><shopping-bag /></el-icon>我的订单
            </template>
            <el-menu-item index="2-1" @click="SalesTicket">购买订单</el-menu-item>
            <el-menu-item index="2-2" @click="SellOrdersList">销售订单</el-menu-item>
            <el-menu-item index="2-3">申请售后</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="3">
            <template #title>
              <el-icon>
                <message />
              </el-icon>我的发布
            </template>
            <el-menu-item index="3-1" @click="SellCar">寄售爱车</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="4">
            <template #title>
              <el-icon>
                <setting />
              </el-icon>用户管理
            </template>
            <el-menu-item index="4-1" @click="manageUser">用户信息</el-menu-item>
            <el-menu-item index="4-2" @click="CarInfo">汽车信息</el-menu-item>
            <el-menu-item index="4-3">管理员信息</el-menu-item>
          </el-sub-menu>
          <el-sub-menu index="5">
            <template #title>
              <el-icon>
                <Promotion />
              </el-icon>审核
            </template>
            <el-menu-item index="5-1" @click="AuditConsignmenet">寄售单审核</el-menu-item>
            <el-menu-item index="5-2" @click="AuditSales">合同审核</el-menu-item>
            <el-menu-item index="5-3">售后</el-menu-item>
          </el-sub-menu> -->
          </el-menu>
        </el-scrollbar>
      </el-aside>

      <el-container>
        <el-header style="text-align: right; font-size: 12px">
          <div class="toolbar">
            <el-dropdown>
              <el-icon style="margin-right: 8px; margin-top: 1px">
                <setting />
              </el-icon>
              <template #dropdown>
                <el-dropdown-menu>
                  <el-dropdown-item @click="backHome">返回主页面</el-dropdown-item>
                  <el-dropdown-item @click="LoginOut">退出登录</el-dropdown-item>
                </el-dropdown-menu>
              </template>
            </el-dropdown>
            <span>{{ Phone }}</span>
          </div>
        </el-header>

        <el-main>
          <el-scrollbar>
            <router-view></router-view>
            <!-- <el-table :data="tableData">
            <el-table-column prop="date" label="Date" width="140" />
            <el-table-column prop="name" label="Name" width="120" />
            <el-table-column prop="address" label="Address" />
          </el-table> -->
          </el-scrollbar>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue'
import { Failed, Menu as Message, Promotion, Setting, ShoppingBag, UserFilled } from '@element-plus/icons-vue'


import { GetPhone as GetPhoneApi, GetMenu } from '@/ts/RequestInterface';
import { useRouter, useRoute } from 'vue-router'
import { ElMessage } from 'element-plus'

const router = useRouter();

const databaseMethod = ref(null);
const choiceFunction = (UrlName: string) => {
  router.replace({ name: UrlName })
}
let Phone = ref("");
const loading = ref(true)
let Menu1 = reactive({
  dt: [{
    id: 0,
    name: "",
    icon: "",
    click: "",
    parentId: 0
  }]

})
const loadData = async () => {
  await GetPhoneApi().then(function (response: any) {
    if (response[0] == "token过期") {
      ElMessage({
        message: '登录过期，请重新登录',
        type: 'warning'
      })
      sessionStorage.removeItem("Cartoken");
      router.push({ name: 'Home' })
    }
    else {
      Phone.value = response[0];
    }

  }).catch(function (error: any) {
    console.log(error);
  });
  //loading.value=false;
}



onMounted(async () => {
  loadData();
  await GetMenu().then(function (response: any) {
    Menu1.dt = response.data;
  }).catch(function (error: any) {
    console.log(error);
  });


  loading.value = false;
});
function backHome() {
  router.push({ name: 'HomePage' })
}
function ChangePass() {
  router.replace({ name: 'ChangePassword' })
}
function ChangeInfo() {
  router.replace({ name: 'ChangeInfo' })
}
const SalesTicket = () => {
  router.replace({ name: 'SalesTicketList' })
}
const SellOrdersList = () => {
  router.replace({ name: 'SellOrdersList' })
}
function CarInfo() {
  router.replace({ name: 'CarInfo' })
}
function SellCar() {//寄售爱车
  router.replace({ name: 'CarInfoList' })
}
function test1() {
  router.replace({ name: 'test1' })
}
function AuditConsignmenet() {
  router.replace({ name: 'AuditConsignmenet' })
}
function AuditSales() {
  router.replace({ name: 'AuditSales' })
}
function LoginOut() {
  sessionStorage.removeItem('Cartoken');
  localStorage.removeItem('Cartoken');
  router.replace({ name: "HomePage" })
}
const manageUser = () => {
  router.replace({ name: 'manageUsers' })
}
</script>

<style scoped>
.layout-container-demo .el-header {
  position: relative;
  background-color: var(--el-color-primary-light-7);
  color: var(--el-text-color-primary);
}

.layout-container-demo .el-aside {
  color: var(--el-text-color-primary);
  background: var(--el-color-primary-light-8);
}

.layout-container-demo .el-menu {
  border-right: none;
}

.layout-container-demo .el-main {
  padding: 0;
}

.layout-container-demo .toolbar {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  right: 20px;
}
</style>
<style>
#Test .el-upload--picture-card {
  display: none;
}
</style>