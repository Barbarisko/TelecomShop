<template>
<h3>Your current {{ props.metric }} usage:</h3>

    <div class="progress" role="progressbar" aria-label="Basic example" :aria-valuenow="valueNow" aria-valuemin="0" :aria-valuemax="props.limit">
        <div class="progress-bar" :style="{width: percentsLeft + '%'}"> {{ valueNow }} </div>
        <div class="progress-bar text-bg-warning" :style="{width: percentsUsed + '%'}"> {{ props.used }} </div>
    </div>
    <div> out of {{ props.limit }}</div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
    metric: String,
    used: Number,
    limit: Number
});

const valueNow = computed(()=> props.limit === undefined || props.used === undefined? 0 : props.limit - props.used)
const percentsLeft = computed(()=> props.limit === undefined || props.used === undefined? 0 : 100 * valueNow.value / props.limit)
const percentsUsed = computed(()=> props.limit === undefined || props.used === undefined? 0 : 100 * props.used / props.limit)

</script>
