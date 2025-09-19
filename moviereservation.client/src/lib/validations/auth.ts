import * as z from "zod"

export const loginSchema = z.object({
  email: z.string().min(1, "Email is required").email("Invalid email address"),
  password: z.string().min(6, "Password is required"),
})

export const registerSchema = z.object({
  name: z.string().min(1, "Name is required"),
  email: z.string().min(1, "Email is required").email("Invalid email address"),
  password: z.string().min(6, "Password must be at least 6 characters"),
})

export const otpSchema = z.object({
  otp: z.string().length(6, "OTP must be 6 digits"),
})