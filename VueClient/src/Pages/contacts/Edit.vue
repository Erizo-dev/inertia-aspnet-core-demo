<template>
  <div>
    <h1 class="mb-8 text-3xl font-bold">
      <Link class="text-indigo-400 hover:text-indigo-600" href="/contacts">Contacts</Link>
      <span class="text-indigo-400 font-medium">/</span>
      {{ form.firstName }} {{ form.lastName }}
    </h1>
    <trashed-message v-if="contact.isDeleted" class="mb-6" @restore="restore"> This contact has been deleted.
    </trashed-message>
    <div class="max-w-3xl bg-white rounded-md shadow overflow-hidden">
      <form @submit.prevent="update">
        <div class="flex flex-wrap -mb-8 -mr-6 p-8">
          <text-input v-model="form.firstName" :error="form.errors.firstName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="First name" />
          <text-input v-model="form.lastName" :error="form.errors.lastName" class="pb-8 pr-6 w-full lg:w-1/2"
            label="Last name" />
          <select-input v-model="form.organizationId" :error="form.errors.organizationId"
            class="pb-8 pr-6 w-full lg:w-1/2" label="Organization">
            <option value=""></option>
            <option v-for="organization in organizations" :key="organization.id" :value="organization.id">
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
        <div class="flex items-center px-8 py-4 bg-gray-50 border-t border-gray-100">
          <button v-if="!contact.deletedAt" class="text-red-600 hover:underline" tabindex="-1" type="button"
            @click="destroy">Delete Contact</button>
          <loading-button :loading="form.processing" class="btn-indigo ml-auto" type="submit">Update
            Contact</loading-button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { defineComponent, PropType } from 'vue'
import { router, Link, useForm } from '@inertiajs/vue3'
import { Contact, Organization } from '../../models/models'
import TextInput from '../../common/TextInput.vue'
import SelectInput from '../../common/SelectInput.vue'
import LoadingButton from '../../common/LoadingButton.vue'
import TrashedMessage from '../../common/TrashedMessage.vue'

const props = defineProps<{
  contact: Contact,
  organizations: Organization[]
}>();
const form = useForm({
  firstName: props.contact.firstName,
  lastName: props.contact.lastName,
  organizationId: props.contact.organizationId,
  email: props.contact.email,
  phone: props.contact.phone,
  address: props.contact.address,
  city: props.contact.city,
  region: props.contact.region,
  country: props.contact.country,
  postalCode: props.contact.postalCode,
});
const update = () => {
  form.put(`/contacts/${props.contact.id}/edit`);
}
const destroy = () => {

  router.delete(`/contacts/${props.contact.id}`);
}
const restore = () => {
  router.post(`/contacts/${props.contact.id}/restore`)
}


</script>
