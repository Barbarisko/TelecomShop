<template>
 <div :id="getId()" class="toast fade show" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <svg class="bd-placeholder-img rounded me-2" width="20" height="20" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"><rect width="100%" height="100%" :fill="color"></rect></svg>
      <strong class="me-auto"> {{ title }}</strong>
      <small>{{timeShown  }}</small>
      <button @click="removeAfterAnimation()" type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      {{ message }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { ToastLevel } from '@/stores/toast';
import * as bootstrap from 'bootstrap';
import { ref, onMounted, computed, type PropType, onUnmounted } from 'vue';

const emit = defineEmits<{
  (e: 'closed'): void
}>()

const timeShown = ref("");

const startTime = new Date()
let toastBootstrap: bootstrap.Toast | null = null;
let stop = false;

onMounted(() => {
  const tEl = document.getElementById(getId()) as Element;
  toastBootstrap = bootstrap.Toast.getOrCreateInstance(tEl, {autohide: props.level == ToastLevel.Info, delay: 5000});
  toastBootstrap.show()

  timer();
});

onUnmounted(() =>{
  stop = true;
  toastBootstrap?.hide()
})

function msToTime(ms: number) {
  let seconds = (ms / 1000);
  let minutes = (ms / (1000 * 60));
  let hours = (ms / (1000 * 60 * 60));
  let days = (ms / (1000 * 60 * 60 * 24));
  let res = "";
  if (seconds < 60) 
  {
    res += seconds.toFixed(0);
    res += seconds == 1 ? " Second" :" Seconds";
  }
  else if (minutes < 60) res += minutes.toFixed(1) + " Minutes";
  else if (hours < 24) res += hours.toFixed(1) + " Hrs";
  else res += days.toFixed(1) + " Days";
  res += " ago";
  return res;
}

function timer()
{
  if(stop)
    return;

  const current = new Date();
  const delta = current.getTime() - startTime.getTime();
  timeShown.value = msToTime(delta);

  if(!toastBootstrap?.isShown())
  {
    removeAfterAnimation();
    return;
  }

  setTimeout(timer, 1000);
}

function removeAfterAnimation(){
  setTimeout(() =>emit('closed'), 1000);
}

  const props = defineProps({
    id: {
      type: Number,
      required: true
    },
    title: {
      type: String,
      required: true
    },
    message: {
      type: String,
      required: true
    },
    level: {
      type: Object as PropType<ToastLevel>,
      required: true
    }
});

const color = computed(() => {
  switch(props.level)
  {
    case ToastLevel.Warning: return "#ffc107"
    case ToastLevel.Error: return "red"
    case ToastLevel.Info: 
    default: 
      return "#007aff"
  }
});

function getId()
{
  return `liveToast`+props.id 
}
</script>
