import { useQuery } from "@tanstack/react-query"

import { permissionsApi } from "@/lib/api-permissions"

export const PERMISSIONS_QUERY_KEY = ["auth", "permissions"]

export const useMyPermissions = (token?: string) =>
  useQuery({
    queryKey: PERMISSIONS_QUERY_KEY,
    queryFn: async () => {
      const res = await permissionsApi.getMyPermissions()
      return res.data
    },
    enabled: !!token,
  })


