<template>
  <div>
    <h1 class="mb-8 text-3xl font-bold">Contacts</h1>
    <div class="flex items-center justify-between mb-6">
      <search-filter v-model="form.search" class="mr-4 w-full max-w-md" @reset="reset">
        <label class="block text-gray-700">Trashed:</label>
        <select v-model="form.trashed" class="form-select mt-1 w-full">
          <option :value="null" />
          <option value="with">With Trashed</option>
          <option value="only">Only Trashed</option>
        </select>
      </search-filter>
      <Link class="btn-indigo" href="/contacts/create">
      <span>Create</span>
      <span class="hidden md:inline">&nbsp;Contact</span>
      </Link>
    </div>
    <div class="bg-white rounded-md shadow overflow-x-auto">
      <table class="w-full whitespace-nowrap">
        <tr class="text-left font-bold">
          <th class="pb-4 pt-6 px-6">Name</th>
          <th class="pb-4 pt-6 px-6">Organization</th>
          <th class="pb-4 pt-6 px-6">City</th>
          <th class="pb-4 pt-6 px-6" colspan="2">Phone</th>
        </tr>
        <tr v-for="contact in contacts.data" :key="contact.id" class="hover:bg-gray-100 focus-within:bg-gray-100">
          <td class="border-t">
            <Link class="flex items-center px-6 py-4 focus:text-indigo-500" :href="`contacts/${contact.id}/edit`">
            {{ contact.firstName }} {{ contact.lastName }}
            <icon v-if="contact.deletedAt" name="trash" class="flex-shrink-0 ml-2 w-3 h-3 fill-gray-400" />
            </Link>
          </td>
          <td class="border-t">
            <Link class="flex items-center px-6 py-4" :href="`contacts/${contact.id}/edit`" tabindex="-1">
              {{ contact.organizationName }}
            </Link>
          </td>
          <td class="border-t">
            <Link class="flex items-center px-6 py-4" :href="`contacts/${contact.id}/edit`" tabindex="-1">
            {{ contact.city }}
            </Link>
          </td>
          <td class="border-t">
            <Link class="flex items-center px-6 py-4" :href="`contacts/${contact.id}/edit`" tabindex="-1">
            {{ contact.phone }}
            </Link>
          </td>
          <td class="w-px border-t">
            <Link class="flex items-center px-4" :href="`contacts/${contact.id}/edit`" tabindex="-1">
            <icon name="cheveron-right" class="block w-6 h-6 fill-gray-400" />
            </Link>
          </td>
        </tr>
        <tr v-if="contacts.data.length === 0">
          <td class="px-6 py-4 border-t" colspan="4">No contacts found.</td>
        </tr>
      </table>
    </div>
    <pagination class="mt-6" :links="pageLinks" />

    <Head :title="title" />
  </div>
</template>

<script lang="ts" setup>
import { defineComponent, PropType } from 'vue'
import { Link as LinkModel, Contact, SearchFilters, LinksDto } from '../../models/models'
import { Link, Head } from '@inertiajs/vue3'
import mapValues from 'lodash/mapValues'
import Icon from '../../common/Icon.vue'
import Pagination from '../../common/Pagination.vue'
import SearchFilter from '../../common/SearchFilter.vue'
import { reactive, computed } from 'vue';
import { watchDebounced } from '@vueuse/core';
import { router } from '@inertiajs/vue3';

type ContactSearch = {
  data: Contact[],
  links: LinksDto,
}

const props = defineProps<{
  filters: SearchFilters,
  contacts: ContactSearch
}>();
const form = reactive({
  search: props.filters.search || '',
  trashed: props.filters.trashed || false
})
const title = "Contacts";

const pageLinks = computed(() => {
  let links: LinkModel[] = [];
  links.push({
    label: 'first',
    url: '/contacts?page=1&perPage=10',
    active: !props.contacts.links.isLastPage
  })
  links.push({
    label: 'last',
    url: `/contacts?page=${1}&perPage=10`,
    active: !props.contacts.links.isLastPage
  })
  links.push({
    label: 'previous',
    url: `/contacts?page=${props.contacts.links.pageNumber - 1}&perPage=10`,
    active: props.contacts.links.hasPreviousPage
  })
  links.push({
    label: 'next',
    url: `/contacts?page=${props.contacts.links.pageNumber + 1}&perPage=10`,
    active: !props.contacts.links.hasNextPage
  })
  return links

})

const reset = () => {
  form.search = '',
    form.trashed = ''
}
const updateSearch = (value: string) => {
  console.log("updating search with ", value)
  form.search = value
}
const url = computed(() => {
  let path = "/contacts"
  let params = []
  if (form.search) {
    params.push(`search=${form.search}`)
  }
  if (form.trashed) {
    params.push(`trashed=${form.trashed}`)
  }
  if (params) {
    path = `${path}?${params.join('&')}`
  }
  console.log("New search url", path)
  return path;

})
watchDebounced([form, url], () => {
  router.get(url.value)
}, { deep: true, debounce: 300 })

</script>
