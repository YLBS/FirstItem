<template>
    <div class="city-navig">
        <div class="city-capital">
            <p v-for="(item, index) in citys" :key="index" @click="clickCapital(item.capital)">{{ item.capital }}</p>
        </div>
        <div class="citys">
            <div v-for="(item, index) in citys" :key="index">
                <p :id="item.capital">{{ item.capital }}:</p>
                <span v-for="(c, index1) in item.arr1" :key="index1" @click="cliskCity(c)">{{ c }}</span>
            </div>
        </div>
    </div>
</template>
<script lang="ts" setup>

import { provinces } from '../../ts/city'
import pinyin2 from 'js-pinyin'

import { ElMessage } from 'element-plus'
import mitss from '@/ts/mitt'
//import { defineEmits } from 'vue';
const Emits=defineEmits(["choiceCity"]);
let citys = sort();

function sort() {
    var capitalLetter = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
    var result = [];
    for (let i = 0; i < capitalLetter.length; i++) {
        var cityss = provinces();
        var object = { capital: "", arr1:[] }
        object.capital = capitalLetter[i];
        var exist = false;
        for (let j = 0; j < cityss.length; j++) {
            var cap = cityss[j];
            if (pinyin2.getFullChars(cap).charAt(0) == capitalLetter[i]) {
                object.arr1.push(cap);
                exist = true;
            }
        }
        if (exist) {
            result.push(object);
        }

    }
    //console.log(result);
    return result;
}
function clickCapital(Capital: string) {
    var PageId = <HTMLAnchorElement>document.querySelector("#" + Capital);
    var c = document.querySelector(".citys");
    if (PageId != null && c != null) {
        c.scrollTo({
            'top': (PageId.offsetTop - 10),
            'behavior': 'smooth'
        })
    }
}

function cliskCity(city: string) {
    //console.log(city);
    const token = sessionStorage.getItem('Cartoken');
    if(token){
        Emits("choiceCity", city);
        mitss.emit('CCC', city)
        mitss.emit('DDD', city)
    }
    else{
        ElMessage({
        message:'请先登录',
        type:'warning'
      })
    }
    
}
</script>
<style>
.city-navig {
    width: 960px;
    height: 632px;
    /*  z-index: 1; */
    position: fixed;
    left: 10%
}

.city-capital {
    width: 30px;
    background-color: antiquewhite;
    float: left;
}

.city-capital p,
.citys p {
    margin-left: 10px;
    height: 12px;
}

.city-capital p:hover,
.citys span:hover {
    color: red;
}

.citys {
    width: 90%;
    height: 100%;
    background-color: rgb(253, 255, 255);
    overflow: auto;
}

.citys span {
    margin-left: 10px;
    width: 200px;
    height: 25px;
    display: inline-block;
}
</style>