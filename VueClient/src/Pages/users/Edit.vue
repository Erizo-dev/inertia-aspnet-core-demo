<template>
  <div>
    <div class="flex justify-start mb-8 max-w-3xl">
      <h1 class="text-3xl font-bold">
        <Link class="text-indigo-400 hover:text-indigo-600" href="/users">Users</Link>
        <span class="text-indigo-400 font-medium">/</span>
        {{ form.firstName }} {{ form.lastName }}
      </h1>
      <img v-if="user.photoPath" class="block ml-4 w-8 h-8 rounded-full" :src="user.photoPath" />
    </div>
    <trashed-message v-if="user.isDeleted" class="mb-6" @restore="restore"> This user has been deleted. </trashed-message>
    <div class="max-w-3xl bg-white rounded-md shadow overflow-hidden">
      <form @submit.prevent="update">
        <div class="flex flex-wrap -mb-8 -mr-6 p-8">
          <text-input v-model="form.firstName" :error="form.errors.firstName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="First name" />
          <text-input v-model="form.lastName" :error="form.errors.lastName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Last name" />
          <text-input v-model="form.email" :error="form.errors.email" class="pb-8 pr-6 w-full lg:w-1/2" label="Email" />
          <text-input v-model="form.password" :error="form.errors.password" class="pb-8 pr-6 w-full lg:w-1/2"
            type="password" autocomplete="new-password" label="Password" />
          <select-input v-model="form.isOwner" :error="form.errors.isOwner" class="pb-8 pr-6 w-full lg:w-1/2" label="Owner">
            <option :value="true">Yes</option>
            <option :value="false">No</option>
          </select-input>
          <!-- <file-input v-model="form.photo" :error="form.errors.photo" class="pb-8 pr-6 w-full lg:w-1/2" type="file" accept="image/*" label="Photo" @input="inputImage"/> -->
        </div>
        <div class="flex items-center px-8 py-4 bg-gray-50 border-t border-gray-100">
          <button v-if="!user.isDeleted" class="text-red-600 hover:underline" tabindex="-1" type="button"
            @click="destroy">Delete User</button>
          <loading-button :loading="form.processing" class="btn-indigo ml-auto" type="submit">Update User</loading-button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { defineComponent, PropType } from 'vue'
import { Link, useForm, router } from '@inertiajs/vue3'
import { User } from '../../models/models'
import Layout from '../../common/Layout.vue'
import TextInput from '../../common/TextInput.vue'
// import FileInput from '../../common/FileInput.vue'
import SelectInput from '../../common/SelectInput.vue'
import LoadingButton from '../../common/LoadingButton.vue'
import TrashedMessage from '../../common/TrashedMessage.vue'

const props = defineProps<{
  user: User
}>();

const form = useForm({
  _method: 'put',
  firstName: props.user.firstName,
  lastName: props.user.lastName,
  email: props.user.email,
  password: '',
  isOwner: props.user.owner,
  photo: null as File | null,
})

const update = () => {
  // const route = this.$route('users.edit', { user_id: this.user.id })
  form.put(`/users/edit/${props.user.id}`, {
    // forceFormData: true,
    onSuccess: () => form.reset('password', 'photo'),
  })
}
const destroy = () => {
  if (confirm('Are you sure you want to delete this user?')) {
    router.delete(`/users/delete/${props.user.id}`)
  }
}
const restore = () => {
  if (confirm('Are you sure you want to restore this user?')) {
    router.put(`/users/restore/${props.user.id}`)
  }
}
const inputImage = (payload: File | null) => {
  form.photo = payload
}
</script>
