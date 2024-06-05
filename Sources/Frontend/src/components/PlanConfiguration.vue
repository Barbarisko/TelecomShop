<template>
<div class="card card-body">
    <ul class="list-group list-group-horizontal" v-for="char in props.plan?.characteristics">
        <li class="list-group-item w-50">{{ char[0] }}</li>
        <li class="list-group-item w-50">{{ char[1] }}</li>
    </ul>
</div>
</template>

<script setup lang="ts">
import PlanModel from '@/models/PlanModel';
import { computed } from 'vue';


const props = defineProps({
    plan: PlanModel,
    hardcodedWidth: 
    {
        default: true,
        type: Boolean
    }
});

const valueNow = computed(()=> props.limit === undefined || props.used === undefined? 0 : props.limit - props.used)
