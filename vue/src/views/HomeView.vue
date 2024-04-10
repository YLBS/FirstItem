<template>
  <div class="common-layout">
    <el-container>
      <el-header> 
        <router-view name="HomeHeader"></router-view>
       <homeHeader></homeHeader>
      </el-header>
      <el-main> 
         <!-- <router-view name="Search" v-if="$route.meta.keepAlive"></router-view> -->
          <router-view name="Navig"></router-view>
      </el-main>
    </el-container>
  </div>
</template>
<script lang="ts" setup>

import { useRoute } from 'vue-router'
const route=useRoute();
//const lt=route.params.lo;
</script>
<style>
.el-main{
  margin-top:15px
}
</style>

