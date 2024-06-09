<template>
<div class="toast-container position-fixed bottom-0 end-0 p-3">
  <div id="liveToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header">
      <img src="..." class="rounded me-2" alt="...">
      <strong class="me-auto">Telecom-Shop</strong>
      <small>Just now</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
     Test message: {{ message }}
    </div>
  </div>
</div>
</template>

<script setup lang="ts">
import bootstrap from 'bootstrap';
import { ref, onMounted } from 'vue';


onMounted(() => {
  const toastInstance = new bootstrap.Toast("");
  toastInstance.show();
});


  const props = defineProps({
    message: {
      type: String,
      required: true
    }
});
</script>

<style scoped>
.toast {
  position: absolute;
  top: 1rem;
  right: 1rem;
  z-index: 1050;
}
</style>