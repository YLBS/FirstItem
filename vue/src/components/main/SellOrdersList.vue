<template>
    <div v-loading="loading">
        <el-table ref="tableRef" row-key="id" :data="tableData.newtable" style="width: 100%"
        @cell-dblclick="doubleClick">
            <el-table-column type="index" :index="indexMethod" />
            <el-table-column prop="sellerName" label="卖方1" width="170" />
            <el-table-column prop="buyerName" label="买方" width="170" />
            <el-table-column prop="createTime" label="创建日期" width="170" />
            <el-table-column prop="carId" label="汽车编号" width="170" />
            <el-table-column prop="state" label="状态" width="170" />
        </el-table>
        <div class="el-pagination">
            <el-pagination layout="prev, pager, next" :total="totalNumber" @current-change="" />
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue'

import { useRouter} from 'vue-router'

import type { TableInstance } from 'element-plus'

import {GetSellOrdersList} from '@/ts/RequestInterface'
const indexMethod = (index: number) => {
    return index + 1
}
const loading = ref(true)
const router = useRouter();
const doubleClick=(row:any,column:any,cell:any,even:any)=>{
    //console.log(row);
    
    let ob = JSON.stringify({ id: row.id })
    router.push({ name: 'NewSalesTicket', state: { ob } })
}

const tableRef = ref<TableInstance>();

let tableData = reactive({
    newtable: [{
        sellerName: 'Tom',
        buyerName: 'Jack',
        createTime: '12343',
        submit: false,
        carId: 0,
        id: 0,
        state: ''
    }]
}
)



const totalNumber = ref(0);

onMounted(async () => {
    var data = await GetSellOrdersList(1,false);
    //console.log(data);
    
    tableData.newtable = data.data
    //totalNumber.value = data.total;
    totalNumber.value = 10;
    loading.value = false
})


</script>

<style scoped>
.el-pagination {
    position: fixed;
    bottom: 12%;
    left: 50%;
}

.el-table .warning-row {
    --el-table-tr-bg-color: var(--el-color-warning-light-9);
}

.el-table .success-row {
    --el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>