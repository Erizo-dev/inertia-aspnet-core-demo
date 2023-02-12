<template>
  <div>
    <h1 class="mb-8 text-3xl font-bold">
      <Link class="text-indigo-400 hover:text-indigo-600" href="/contacts">Contacts</Link>
      <span class="text-indigo-400 font-medium">/</span> Create
    </h1>
    <div class="max-w-3xl bg-white rounded-md shadow overflow-hidden">
      <form @submit.prevent="store">
        <div class="flex flex-wrap -mb-8 -mr-6 p-8">
          <text-input v-model="form.firstName" :error="form.errors.firstName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="First name" />
          <text-input v-model="form.lastName" :error="form.errors.lastName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Last name" />
          <select-input v-model="form.organizationId" :error="form.errors.organizationId"
            class="pb-8 pr-6 w-full lg:w-1/2" label="Organization">
            <option value=""></option>
            <option v-for="organization in props.organizations" :key="organization.id" :value="organization.id">
              {{ organization.name }}</option>
          </select-input>
          <text-input v-model="form.email" :error="form.errors.email" class="pb-8 pr-6 w-full lg:w-1/2" label="Email" />
          <text-input v-model="form.phone" :error="form.errors.phone" class="pb-8 pr-6 w-full lg:w-1/2" label="Phone" />
          <text-input v-model="form.address" :error="form.errors.address" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Address" />
          <text-input v-model="form.city" :error="form.errors.city" class="pb-8 pr-6 w-full lg:w-1/2" label="City" />
          <text-input v-model="form.region" :error="form.errors.region" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Province/State" />
          <select-input v-model="form.country" :error="form.errors.country" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Country">
            <option value=""></option>
            <option value="CA">Canada</option>
            <option value="US">United States</option>
          </select-input>
          <text-input v-model="form.postalCode" :error="form.errors.postalCode" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Postal code" />
        </div>
        <div class="flex items-center justify-end px-8 py-4 bg-gray-50 border-t border-gray-100">
          <loading-button :loading="form.processing" class="btn-indigo" type="submit">Create Contact</loading-button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { defineComponent, PropType } from 'vue'
import { Link, useForm } from '@inertiajs/vue3'
import { Organization } from '../../models/models'
import TextInput from '../../common/TextInput.vue'
import SelectInput from '../../common/SelectInput.vue'
import LoadingButton from '../../common/LoadingButton.vue'

const props = defineProps<{
  organizations: Organization[],
}>();
const form = useForm({
  firstName: '',
  lastName: '',
  organizationId: '',
  email: '',
  phone: '',
  address: '',
  city: '',
  region: '',
  country: '',
  postalCode: '',
});

const store = () => {
  form.post('/contacts/create');
}

</script>
