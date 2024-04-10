<template>
  <div class="about">
    <h1 @click="cc">This is an about page</h1>
  </div>
</template>
<script lang="ts" setup>

import { useRouter,useRoute } from 'vue-router'
const router=useRouter();
function cc(){
  router.replace({path:'/'})
}
const route=useRoute();

</script>
