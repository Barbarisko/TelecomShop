<template>
    <div class="card" :style="hardcodedWidth ? 'width: 18rem;': ''">
        <div class="card-body">
        <h5 class="card-title">{{ plan?.name }}</h5>
        <p class="card-text">{{plan?.description}}</p>
        <p class="fw-semibold">{{plan?.recurrentTotal}}</p>
        <p class="d-inline-flex gap-1">
            <button class="btn btn-primary" type="button" data-bs-toggle="collapse" :data-bs-target="`#plan${plan?.id}`" aria-expanded="false" :aria-controls="`#plan${plan?.id}`">
                Show characteristics
                        </button>
        </p>
        <div class="collapse" :id="`plan${plan?.id}`">
            <div class="card card-body">
                <ul class="list-group list-group-horizontal" v-for="char in props.plan?.characteristics">
                    <li class="list-group-item w-50">{{ char[0] }}</li>
                    <li class="list-group-item w-50">{{ char[1] }}</li>
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


</script>
