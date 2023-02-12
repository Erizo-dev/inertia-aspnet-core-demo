<template>
  <div>
    <h1 class="mb-8 font-bold text-3xl">Organizations</h1>
    <div class="mb-6 flex justify-between items-center">
      <search-filter :maxWidth="300" w-full max-w-md mr-4 @reset="reset" v-model="form.search">
        <label class="block text-gray-700">Trashed:</label>
        <select v-model="form.trashed" class="mt-1 w-full form-select">
          <option :value="null" />
          <option value="with">With Trashed</option>
          <option value="only">Only Trashed</option>
        </select>
      </search-filter>
      <Link class="btn-indigo" :href="'organizations/create'" :method="Method.GET">
      <span>Create Organization</span>
      </Link>
    </div>
    <div class="bg-white rounded-md shadow overflow-x-auto">
      <table class="w-full whitespace-nowrap">
        <thead>
          <tr class="text-left font-bold">
            <th class="px-6 pt-6 pb-4">Name</th>
            <th class="px-6 pt-6 pb-4">City</th>
            <th class="px-6 pt-6 pb-4" colspan="2">Phone</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="organization in organizations.data" :key="organization.id"
            class="hover:bg-gray-100 focus-within:bg-gray-100">
            <td class="border-t">
              <Link class="px-6 py-4 flex items-center focus:text-indigo-500"
                :href="`organizations/${organization.id}/edit`">
              {{ organization.name }}
              <icon v-if="organization?.isDeleted" name="trash" class="flex-shrink-0 w-3 h-3 fill-gray-400 ml-2" />
              </Link>
            </td>
            <td class="border-t">
              <Link class="px-6 py-4 flex items-center" :href="`organizations/${organization.id}/edit`" tabindex="-1">
              {{ organization.city }}
              </Link>
            </td>
            <td class="border-t">
              <Link class="px-6 py-4 flex items-center" :href="`organizations/${organization.id}/edit`" tabindex="-1">
              {{ organization.phone }}
              </Link>
            </td>
            <td class="border-t w-px">
              <Link class="px-4 flex items-center" :href="`organizations/${organization.id}/edit`" tabindex="-1">
              <icon name="cheveron-right" class="block w-6 h-6 fill-gray-400" />
              </Link>
            </td>
          </tr>
          <tr v-if="organizations.data.length === 0">
            <td class="border-t px-6 py-4" colspan="4">No organizations found.</td>
          </tr>
        </tbody>
      </table>
    </div>
    <!-- <pagination class="mt-6" :links="organizations.links" /> -->
    <pagination class="mt-6" :links="pageLinks" />
  </div>

  <Head :title="title" />
</template>

<script lang="ts" setup>
import { defineComponent, PropType } from 'vue'
import { Link as LinkModel, LinksDto, Organization, SearchFilters } from '../../models/models'
import { Link } from '@inertiajs/vue3'
import Icon from '../../common/Icon.vue'
import Pagination from '../../common/Pagination.vue'
import SearchFilter from '../../common/SearchFilter.vue'
import { Head } from '@inertiajs/vue3'
import { ref, reactive, watchEffect, watch, computed } from 'vue'
import { router } from '@inertiajs/vue3'
import {watchDebounced} from '@vueuse/core'
import { Method } from '@inertiajs/core'

type OrganizationSearch = {
  data: Organization[],
  links: LinksDto,
}

const props = defineProps<{
  organizations: OrganizationSearch,
  filters: SearchFilters
}>();
const form = reactive({
  search: props.filters.search || '',
  trashed: props.filters.trashed || false
})

const title = "Organizations";

const pageLinks = computed(() => {
  let links: LinkModel[] = [];
  links.push({
    label: 'first',
    url: '/organizations?page=1&perPage=10',
    active: !props.organizations.links.isLastPage
  })
  links.push({
    label: 'last',
    url: `/organizations?page=${1}&perPage=10`,
    active: !props.organizations.links.isLastPage
  })
  links.push({
    label: 'previous',
    url: `/organizations?page=${props.organizations.links.pageNumber -1 }&perPage=10`,
    active: props.organizations.links.hasPreviousPage
  })
  links.push({
    label: 'next',
    url: `/organizations?page=${props.organizations.links.pageNumber +1 }&perPage=10`,
    active: !props.organizations.links.hasNextPage
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
  let path = "/organizations"
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
}, { deep: true, debounce: 300})

</script>
