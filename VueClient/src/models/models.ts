export type Contact = {
  id: number
  name: string
  organizationId: number
  organization: Record<string, unknown>
  firstName: string
  lastName: string
  email?: string
  phone?: string
  address?: string
  city?: string
  region?: string
  country?: string
  postalCode?: string
  deletedAt?: string
  isDeleted: boolean
  organizationName: string
}

export type Organization = {
  id: number
  name: string
  email?: string
  phone?: string
  address?: string
  city?: string
  region?: string
  country?: string
  postalCode?: string
  isDeleted: boolean
  contacts: Contact[]
}

export type User = {
  id: number
  firstName: string
  lastName: string
  email?: string
  photoPath?: string
  owner?: boolean
  deletedAt?: string,
  isDeleted: boolean
}

export type Link = {
  url: string | null
  label: string
  active?: boolean
}
export type LinksDto = {
  isFirstPage : boolean,
  isLastPage : boolean,
  pageCount: number,
  pageNumber: number,
  pageSize: number,
  hasNextPage: boolean,
  hasPreviousPage: boolean
}

export type SearchFilters = {
  search?: string
  trashed?: string
  role?: string
}
