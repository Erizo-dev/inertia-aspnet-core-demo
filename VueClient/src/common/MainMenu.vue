<template>
  <div>
    <div class="mb-4">
      <Link class="flex items-center group py-3" href="/">
        <icon name="dashboard" class="w-4 h-4 mr-2 fill-white" />
        <div class="text-white">Dashboard</div>
      </Link>
    </div>
    <div class="mb-4">
      <Link class="flex items-center group py-3" href="/organizations">
        <icon name="office" class="w-4 h-4 mr-2 fill-white" />
        <div class="text-white">Organizations</div>
      </Link>
    </div>
    <div class="mb-4">
      <Link class="flex items-center group py-3" href="/contacts">
        <icon name="users" class="w-4 h-4 mr-2 fill-white" />
        <div class="text-white">Contacts</div>
      </Link>
    </div>
    <div class="mb-4">
      <Link class="flex items-center group py-3" href="/">
        <icon name="printer" class="w-4 h-4 mr-2 fill-white" />
        <div class="text-white">Reports</div>
      </Link>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { Link } from '@inertiajs/vue3'
import Icon from './Icon.vue'

</script>
