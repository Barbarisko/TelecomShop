<template>
    <div class="card" :style="hardcodedWidth ? 'width: 18rem;': ''">
        <div class="card-body">
        <h5 class="card-title">{{ plan?.name }}</h5>
        <p class="card-text">{{plan?.description}}</p>
        <p class="fw-semibold">{{plan?.recurrentTotal}}</p>
        <p class="d-inline-flex gap-1">
            <button class="btn btn-primary" type="button" data-bs-toggle="collapse" data-bs-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">
                Show characteristics
                        </button>
        </p>
        <div class="collapse" id="collapseExample">
            <div class="card card-body">
                <ul class="list-group list-group-horizontal" v-for="charKey in characteristicsKeys">
                    <li class="list-group-item">{{ charKey }}</li>
                    <li class="list-group-item">{{ (props.plan?.characteristics as any)[charKey] }}</li>
                </ul>
            </div>
        </div>
    </div>
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
const characteristicsKeys = computed(() => {
    var chars: Array<[string, string]> = [];
    return Object.keys(props.plan?.characteristics as any);
})
</script>
