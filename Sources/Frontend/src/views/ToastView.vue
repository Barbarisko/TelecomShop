<template>
<div class="toast-container position-fixed bottom-0 end-0 p-3">
    <Toast v-for="t in toastStore.data" :id ="t[0]" :level="t[1].level" :title="t[1].title" :message="t[1].message" @closed="OnClose(t[0])" />
</div>
</template>

<script setup lang="ts">
import Toast from '@/components/Toast.vue';
import { useToastStore } from '@/stores/toast';
const toastStore = useToastStore();

function OnClose(id: number)
{
  toastStore.data.delete(id);
}
</script>